Write-Host "--- Git Helper Initialized ---" -ForegroundColor Cyan

# 1. Thêm file vào .git/info/exclude (ignore local, không ảnh hưởng người khác)
function g-exclude {
    param([string]$path)

    $excludeFile = ".git/info/exclude"

    if ($path) {
        # Chuẩn hóa đường dẫn: Đổi tất cả dấu gạch chéo ngược (\) thành gạch chéo xuôi (/)
        $normalizedPath = $path -replace "\\", "/"

        # 1. Kiểm tra và thêm dòng mới nếu file exclude bị thiếu
        if (Test-Path $excludeFile) {
            $rawContent = Get-Content $excludeFile -Raw
            if ($rawContent -and -not $rawContent.EndsWith("`n")) {
                "" | Out-File -FilePath $excludeFile -Append -Encoding ascii
            }
        }

        # 2. Thêm path đã chuẩn hóa vào file
        $normalizedPath | Out-File -FilePath $excludeFile -Append -Encoding ascii
        Write-Host "Added $normalizedPath to $excludeFile" -ForegroundColor Yellow
    }
    else {
        Write-Host "Usage: g-exclude <file-path>" -ForegroundColor Red
    }
}

# 2. Skip Worktree (Hỗ trợ nhiều file)
function g-skip {
    param(
        [Parameter(ValueFromRemainingArguments = $true)]
        [string[]]$paths
    )

    if (-not $paths) {
        Write-Host "Usage: g-skip <file1> <file2> ..." -ForegroundColor Red
        return
    }

    foreach ($path in $paths) {
        git update-index --skip-worktree $path
        Write-Host "Skipped: $path" -ForegroundColor Green
    }
}

# 3. Unskip Worktree (file hoặc folder)
function g-unskip {
    param([string]$path)

    if (-not $path) {
        Write-Host "Usage: g-unskip <file-or-folder>" -ForegroundColor Red
        return
    }

    $normalizedPath = $path -replace "\\", "/"

    $files = git ls-files -- "$normalizedPath" "$normalizedPath/*"

    if (-not $files) {
        Write-Host "No tracked files found for: $normalizedPath" -ForegroundColor Yellow
        return
    }

    foreach ($file in $files) {
        git update-index --no-skip-worktree -- "$file"
    }

    Write-Host "Unskipped:" -ForegroundColor Green
    $files
}

# 4. Liệt kê file đang skip
function g-skipped {
    Write-Host "Files currently skipped:" -ForegroundColor Cyan
    git ls-files -v | Where-Object { $_.StartsWith("S") }
}

# 5. Resolve conflict - chọn CURRENT (ours)
function g-accept-current {
    param([string]$path)

    git checkout --ours $path
    git add $path
    Write-Host "Accepted CURRENT (ours) for $path" -ForegroundColor Green
}

# 6. Resolve conflict - chọn INCOMING (theirs)
function g-accept-incoming {
    param([string]$path)

    git checkout --theirs $path
    git add $path
    Write-Host "Accepted INCOMING (theirs) for $path" -ForegroundColor Green
}

# 7. Abort merge đang dở
function g-merge-abort {
    git merge --abort
    Write-Host "Merge aborted." -ForegroundColor Yellow
}

# 8. Reset cứng branch hiện tại (xoá mọi thay đổi local)
function g-reset-hard {
    git reset --hard
    Write-Host "Hard reset completed." -ForegroundColor Yellow
}

# 9. Reset branch hiện tại về remote (ví dụ origin/Ducan)
function g-reset-remote {
    param([string]$branch)

    if (-not $branch) {
        $branch = git rev-parse --abbrev-ref HEAD
    }

    git fetch origin
    git reset --hard origin/$branch

    Write-Host "Branch reset to origin/$branch" -ForegroundColor Yellow
}

# 10. Cất tạm thay đổi (Stash) - Có thể ghi chú thêm lý do
function g-stash {
    param([string]$message)

    if ($message) {
        git stash push -m $message
        Write-Host "Stashed changes with message: '$message'" -ForegroundColor Yellow
    }
    else {
        git stash
        Write-Host "Stashed changes (no message)." -ForegroundColor Yellow
    }
}

# 11. Lấy lại thay đổi vừa cất (Stash pop)
function g-stash-pop {
    git stash pop
    Write-Host "Applied and removed the latest stash." -ForegroundColor Green
}

# 12. Xem danh sách các gói đã cất
function g-stash-list {
    Write-Host "Current stashes:" -ForegroundColor Cyan
    git stash list
}
# 14. Tự động skip tất cả file .lscache đang bị modified
function g-skip-cache {
    $cacheFiles = git ls-files -m | Where-Object { $_ -like "*.lscache" }
    
    if (-not $cacheFiles) {
        Write-Host "No .lscache files to skip." -ForegroundColor Gray
        return
    }

    foreach ($file in $cacheFiles) {
        git update-index --skip-worktree $file
        Write-Host "Auto-skipped: $file" -ForegroundColor Green
    }
}

# 15. Thiết lập nhanh cho project mới (Gộp tất cả lệnh ignore/skip)
function g-init-project {
    Write-Host "--- Initializing Local Git Settings ---" -ForegroundColor Cyan

    # Nhóm 1: Dùng g-exclude cho các file/thư mục Untracked (chưa từng commit)
    $excludes = @(
        "ch010-marble-blast.slnx",
        "Packages/com.singularitygroup.hotreload/",
        "Assets/FindReference2/",
        "Assets/FindReference2.meta",
        "Assets/FR2_Cache.asset",
        "Assets/FR2_Cache.asset.meta",
        "Assets/vFavorites/",
        "Assets/vFavorites.meta",
        "*.lscache"
    )
    foreach ($item in $excludes) {
        g-exclude $item
    }

    # Nhóm 2: Dùng g-skip cho các file hệ thống đã có trên Git (Tracked)
    $skips = @(
        "Packages/packages-lock.json",
        "ProjectSettings/EntitiesClientSettings.asset",
        "ProjectSettings/Packages/com.eflatun.scenereference/Settings.json",
        "UserSettings/EditorUserSettings.asset"
    )
    g-skip $skips

    Write-Host "--- Done! Your Workspace is clean. ---" -ForegroundColor Green
}

Write-Host "Commands:" -ForegroundColor Gray
Write-Host "g-exclude, g-skip, g-unskip, g-skipped" -ForegroundColor Gray
Write-Host "g-accept-current, g-accept-incoming" -ForegroundColor Gray
Write-Host "g-merge-abort, g-reset-hard, g-reset-remote" -ForegroundColor Gray
Write-Host "g-stash, g-stash-pop, g-stash-list" -ForegroundColor Gray
Write-Host "g-skip-cache" -ForegroundColor Gray