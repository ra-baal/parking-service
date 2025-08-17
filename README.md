# parking-service

## Clean Architecture 

```mermaid
graph TD
	Api[API]
	Dom[Domain]
	Inf[Infrastructure]

    Api --> Dom
	Api --> Inf

    Inf --> Dom
```
