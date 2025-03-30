export JWT_SECRET_BASE64=$(echo -n "$JWT_SECRET" | base64 -w 0)
export STORAGE_CONNECTION_STRING_BASE64=$(echo -n "$STORAGE_CONNECTION_STRING" | base64 -w 0)
export DBCONN_BASE64=$(echo -n "$DBCONN" | base64 -w 0)
export METRICS_PASSWORD_BASE64=$(echo -n "$METRICS_PASSWORD" | base64 -w 0)
export BLOB_CONTAINER_BASE64=$(echo -n "$BLOB_CONTAINER" | base64 -w 0)
export PRIVATE_BLOB_CONTAINER_BASE64=$(echo -n "$PRIVATE_BLOB_CONTAINER" | base64 -w 0)
export TOKEN_EXPIRES_BASE64=$(echo -n "$TOKEN_EXPIRES" | base64 -w 0)
export BASE_URL_BASE64=$(echo -n "$BASE_URL" | base64 -w 0)


envsubst < src/Kube/service-clock.secrets.yml > src/Kube/service-clock.secrets.tmp.yml
envsubst < src/Kube/service-clock.deployment.yml > src/Kube/service-clock.deployment.tmp.yml
envsubst < src/Kube/service-clock.ingress.yml > src/Kube/service-clock.ingress.tmp.yml
envsubst < src/Kube/service-clock.service.yml > src/Kube/service-clock.service.tmp.yml