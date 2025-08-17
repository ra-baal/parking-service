# parking-service

## Clean Architecture 

```mermaid
graph LR
	Api[API]
	Dom[Domain]
	Inf[Infrastructure]

    Api --> Dom
	Api --> Inf

    Inf --> Dom
```
