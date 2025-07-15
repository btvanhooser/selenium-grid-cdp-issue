# selenium-grid-cdp-issue

## Issue

Running this basic test to manipulate the network during a simple test. This test works without issue when running against a locally running browser (ie: ChromeDriver) or a locally running Selenium Server instance (executing the .jar program directly). When running this exact same flow in a Docker or K8s (Helm) context, suddenly the DevTools connection can't be made.

## Running

### Local

Set the "Browser" property on the main "SeleniumGridCdpTests" to "LocalInstall". Both tests should pass.

### Docker

Ensure Docker/Podman is running and execute "docker compose up -d" on the base repo directory. Set the "Browser" property on the main "SeleniumGridCdpTests" to "DockerGrid". Only the test without the CDP stuff works.

### Kubernetes

Ensure you have a running cluster. For my testing, I was using Podman Desktop with the "Kind" cluster running. Then run the following steps:
- helm repo add docker-selenium https://www.selenium.dev/docker-selenium
- helm repo update
- helm install selenium-grid docker-selenium/selenium-grid --version 0.43.2 --values values.yaml

For Podman Desktop, I also needed to forward the "4444" port to ensure it's exposed. For me, that defaulted to port 50000. Please adjust to whatever makes sense for you.

Set the "Browser" property on the main "SeleniumGridCdpTests" to "DockerGrid". Only the test without the CDP stuff works.