Get-Item fileThatDoesNotExist.txt

if (-Not $?) {
    Return 1
}

Return 0
