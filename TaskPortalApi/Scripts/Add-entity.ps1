function Add-Entity() {
 [CmdletBinding()]
 param(
 $table,
 [String]$partitionKey,
 [String]$rowKey,
 [String]$code,
 [String]$description
)

$entity = New-Object -TypeName
Microsoft.WindowsAzure.Storage.Table.DynamicTableEntity -ArgumentList 
$partitionKey, $rowKey
 $entity.Properties.Add("partitionKey", $partitionKey)
 $entity.Properties.Add("rowKey", $rowKey)
 $entity.Properties.Add("code", $code)
 $entity.Properties.Add("description", $description)

$result =
$table.CloudTable.Execute([Microsoft.WindowsAzure.Storage.Table.TableOperation]::Insert($entity))
}

$StorageAccountName = "<StorageAccountName>"
$StorageAccountKey = Get-AzureStorageKey -StorageAccountName
$StorageAccountName
$Ctx = New-AzureStorageContext $StorageAccountName -StorageAccountKey
$StorageAccountKey.Primary
$TableName = "<TableName>"
$table = Get-AzureStorageTable –Name $TableName -Context $Ctx -ErrorAction
Ignore
#Add multiple entities to a table.
Add-Entity -Table $table -PartitionKey Partition1 -RowKey Row1 -code Code1 -Id 1 -description abc
Add-Entity -Table $table -PartitionKey Partition2 -RowKey Row2 -code Code1 -Id 2 -description abc 
Add-Entity -Table $table -PartitionKey Partition3 -RowKey Row3 -code Code1 -Id 3 -description xyz 
Add-Entity -Table $table -PartitionKey Partition4 -RowKey Row4 -code Code1 -Id 4 -description xyz 
