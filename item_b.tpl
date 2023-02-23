{
  "name": "item_a",
  "extends": "item",
  "components": [
    {
      "flyweight": true,
      "overwrite": true,
      "component": {
        "class": "ItemDefinition",
        "category": "weapon",
        "stackable": true,
        "stackSize": 50
      }
    },
    {
      "component": {
        "class": "Location",
        "position": [1,0]
      }
    }
  ],
  "behaviours": [
    "foo",
    "bar"
  ]}