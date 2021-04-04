Install-Module AzTable

# Sign in to Azure
Add-AzAccount

# Set the region
$location = "westeurope"

#Set resource group
$resourceGroup = "test3"

#Create storage account
$storageAccountName = "taskPortalManager"
$storageAccount = New-AzStorageAccount -ResourceGroupName $resourceGroup `
-Name $storageAccountName `
-Location $location `
-SkuName Standard_LRS `
-Kind Storage


# Obtain the storage account context
$context = $storageAccount.Context


# Create the table => Project by default
$PROJECT_tableName = "Project"
New-AzStorageTable -Name $tableName -Context $context

#$TASK_tableName = "Task"
#New-AzStorageTable -Name $tableName -Context $context


# List tables
Get-AzStorageTable -Context $context | select Name


#Reference your table => Project by default
$PROJECT_storageTable = Get-AzStorageTable -Name $PROJECT_tableName -Context $context
# $TASK_storageTable = Get-AzStorageTable -Name $TASK_tableName -Context $context


# Reference the CloudTable property of your table => Project by default
$PROJECT_cloudTable = (Get-AzStorageTable -Name $PROJECT_tableName -Context $context).CloudTable
#$TASK_cloudTable = (Get-AzStorageTable -Name $TASK_tableName -Context $context).CloudTable


# Add entities for Project table
Add-AzTableRow `
    -Table $cloudTable `
    -PartitionKey "ProjectTest0" `
    -RowKey ("1111") -property @{"Description"="TestDescription";"Code"="TestCode"}

Add-AzTableRow `
    -Table $cloudTable `
    -PartitionKey "ProjectTest1" `
    -RowKey ("1112") -property @{"Description"="TestDescription1";"Code"="TestCode1"}


# Query for projects
Get-AzTableRow -Table $cloudTable -PartitionKey "ProjectTest0" | ft



    