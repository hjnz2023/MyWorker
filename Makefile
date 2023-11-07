build:
	docker build -t myworker .
start:
	docker run --rm -it -e NEW_RELIC_LICENSE_KEY=$${NEW_RELIC_LICENSE_KEY} -e NEW_RELIC_METADATA_COMMIT=$$(git rev-parse HEAD) myworker

syntax_check:
	echo $$(git rev-parse HEAD)
	echo $${NEW_RELIC_LICENSE_KEY}