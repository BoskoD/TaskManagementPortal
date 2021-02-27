$StorageAccountName = "mystorageaccount"
$StorageAccountKey = "mystoragekey"
$Ctx = New-AzureStorageContext $StorageAccountName -StorageAccountKey
$StorageAccountKey

#Create table
$tabName = "<TableName>"
New-AzureStorageTable –Name $tabName –Context $Ctx#Get-table$tabName = "<TableName>"
Get-AzureStorageTable –Name $tabName –Context $Ctx#Delete table$tabName = "<TableName>"
Remove-AzureStorageTable –Name $tabName –Context $Ctx