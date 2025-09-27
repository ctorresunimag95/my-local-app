@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param sku string = 'Standard'

resource serviceBus 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: take('serviceBus-${uniqueString(resourceGroup().id)}', 50)
  location: location
  properties: {
    disableLocalAuth: true
  }
  sku: {
    name: sku
  }
  tags: {
    'aspire-resource-name': 'serviceBus'
  }
}

output serviceBusEndpoint string = serviceBus.properties.serviceBusEndpoint

output name string = serviceBus.name