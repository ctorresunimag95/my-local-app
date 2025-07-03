import { EnvironmentProviders, provideAppInitializer } from '@angular/core';
import {
    BatchSpanProcessor,
    ConsoleSpanExporter,
    SimpleSpanProcessor,
} from '@opentelemetry/sdk-trace-base';
import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-proto';
import { defaultResource, resourceFromAttributes } from '@opentelemetry/resources';
import { ATTR_SERVICE_NAME, ATTR_SERVICE_VERSION } from '@opentelemetry/semantic-conventions';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';

export function provideInstrumentation(): EnvironmentProviders {
    return provideAppInitializer(() => {
        // Configure our resource
        const resource = defaultResource().merge(
            resourceFromAttributes({
                [ATTR_SERVICE_NAME]: 'cinema-web',
                [ATTR_SERVICE_VERSION]: '1.0.0',
            }),
        );

        const provider = new WebTracerProvider({
            resource,
            spanProcessors: [
                new SimpleSpanProcessor(new ConsoleSpanExporter()), // Batch traces before sending them to Collector
                new BatchSpanProcessor(
                    new OTLPTraceExporter({
                        url: `${window.origin}/v1/traces`,
                    }),
                ),// Supports correlating asynchronous operations
            ]
        });

        provider.register({
            contextManager: new ZoneContextManager(),
        }); // Register instrumentations to automatically capture traces from

        registerInstrumentations({
            instrumentations: [
                getWebAutoInstrumentations({
                    '@opentelemetry/instrumentation-document-load': {},
                    '@opentelemetry/instrumentation-user-interaction': {},
                    '@opentelemetry/instrumentation-fetch': {},
                    '@opentelemetry/instrumentation-xml-http-request': {},
                }),
            ],
        });
    });
}