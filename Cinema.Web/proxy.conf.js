module.exports = {
    "/gateway": {
        target:
            process.env["services__gateway__https__0"] ||
            process.env["services__gateway__http__0"],
        secure: process.env["NODE_ENV"] !== "development",
        pathRewrite: {
            "^/gateway": "",
        },
    },
    '/v1/traces': {
        target: process.env['OTEL_EXPORTER_OTLP_ENDPOINT'],
        secure: process.env['NODE_ENV'] !== 'development',
        headers: parseHeaders(process.env['OTEL_EXPORTER_OTLP_HEADERS']),
    }
};

function parseHeaders(s) {
  const headers = s.split(','); // Split by comma
  const result = {};

  headers.forEach(header => {
      const [key, value] = header.split('='); // Split by equal sign
      result[key.trim()] = value.trim(); // Add to the object, trimming spaces
  });

  return result;
}