# Entity Framework Batch Extensions

EF DbContext extensions to optimize batch operations

## Tech Goals

* Extend existing DbContext with convenient API
* Achieve performance comparable to alternatives

## Important Notes

* For now mapping works only for primitives (and probably will remain that way). Be cautious that types like String and Decimal are not primitives
* Update will set all values from passed object to entities with passed ids

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
