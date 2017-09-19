Get-Item fileDoesNotExist.txt

if (-Not $?) {
    Return 1
}

Return 0