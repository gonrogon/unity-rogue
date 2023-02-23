[
    {
          "name": "item",
          "components": [
            {
              "flyweight": true,
              "component": {
                "class": "ItemDefinition",
                "category": "weapon",
                "stackable": true,
                "stackSize": 100
              }
            },
            {
              "component": {
                "class": "Item",
                "itemId": -1,
                "amount": 1
              }
            }
          ],
          "behaviours": ["asd"]
    },
    {
      "name": "item_a",
      "extends": "item",
      "components": [
        {
          "flyweight": true,
          "overwrite": true,
          "component": {
            "class": "ItemDefinition",
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
      "behaviours": ["foo", "bar"]
    }
]