@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param cosmosdb_outputs_name string

param principalId string

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2024-08-15' existing = {
  name: cosmosdb_outputs_name
}

resource cosmosDb_roleDefinition 'Microsoft.DocumentDB/databaseAccounts/sqlRoleDefinitions@2024-08-15' existing = {
  name: '00000000-0000-0000-0000-000000000002'
  parent: cosmosDb
}

resource cosmosDb_roleAssignment 'Microsoft.DocumentDB/databaseAccounts/sqlRoleAssignments@2024-08-15' = {
  name: guid(principalId, cosmosDb_roleDefinition.id, cosmosDb.id)
  properties: {
    principalId: principalId
    roleDefinitionId: cosmosDb_roleDefinition.id
    scope: cosmosDb.id
  }
  parent: cosmosDb
}