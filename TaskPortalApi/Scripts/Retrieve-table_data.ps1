$StorageAccountName = "AccountName"
$StorageAccountKey = Get-AzureStorageKey -StorageAccountName
$StorageAccountName
$Ctx = New-AzureStorageContext –StorageAccountName $StorageAccountName -
StorageAccountKey $StorageAccountKey.Primary;
$TableName = "<TableName>"

#Get a reference to a table.
$table = Get-AzureStorageTable –Name $TableName -Context $Ctx

#Create a table query.
$query = New-Object Microsoft.WindowsAzure.Storage.Table.TableQuery

#Define columns to select.
$list = New-Object System.Collections.Generic.List[string]
$list.Add("RowKey")
$list.Add("PartitionKey")
$list.Add("<More columns>")

#Set query details.
$query.SelectColumns = $list
$query.TakeCount = 20

#Execute the query.
$entities = $table.CloudTable.ExecuteQuery($query)

#Display entity properties with the table format.
$entities | Format-Table PartitionKey, RowKey, @{ Label = "RowKey";
Expression={$_.Properties["RowKey"].StringValue}}, @{ Label = "PartitionKey";
Expression={$_.Properties[“PartitionKey”].StringValue}} -AutoSize 