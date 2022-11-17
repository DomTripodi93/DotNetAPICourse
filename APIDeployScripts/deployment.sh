
az login

az webapp up --sku F1 --name "AppName" --os-type linux

az webapp delete --name "AppName"

az group delete --name "GroupName"

az sql db delete --name "DatabaseName"

az sql server delete --name "server-name"
