
export JWT_SECRET_BASE64=$(echo -n "$JWT_SECRET" | base64)
export STORAGE_CONNECTION_STRING_BASE64=$(echo -n "$STORAGE_CONNECTION_STRING" | base64)
export DBCONN_BASE64=$(echo -n "$DBCONN" | base64)
export METRICS_PASSWORD_BASE64=$(echo -n "$METRICS_PASSWORD" | base64)
export BLOB_CONTAINER_BASE64=$(echo -n "$BLOB_CONTAINER" | base64)
export PRIVATE_BLOB_CONTAINER_BASE64=$(echo -n "$PRIVATE_BLOB_CONTAINER" | base64)
export TOKEN_EXPIRES_BASE64=$(echo -n "$TOKEN_EXPIRES" | base64)
export BASE_URL_BASE64=$(echo -n "$BASE_URL" | base64)

envsubst < src/Kube/service-clock.secrets.yml > src/Kube/service-clock.secrets.yml
envsubst < src/Kube/service-clock.deployment.yml > src/Kube/service-clock.deployment.yml
envsubst < src/Kube/service-clock.ingress.yml > src/Kube/service-clock.ingress.yml
envsubst < src/Kube/service-clock.service.yml > src/Kube/service-clock.service.yml
