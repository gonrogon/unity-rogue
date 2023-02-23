{
  "templates": {
    "item": {
      "name": "item",
      "extends": "",
      "components": [
        {
          "Flyweight": true,
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
      "behaviours": []
    },
    "item_a": {
      "name": "item_a",
      "extends": "item",
      "components": [
        {
          "Flyweight": true,
          "Overwrite": true,
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
      "behaviours": []
    }
  }
}