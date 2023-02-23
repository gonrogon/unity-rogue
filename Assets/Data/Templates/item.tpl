[
    {
        "name": "item",
        "components": [
            {
                "flyweight": true,
                "component": ["ItemDecl", {"category": "item", "type": "item", "price": 0, "condition": 100}]
            },
            {
                "component": ["Item"]
            },
            {
                "component": ["Name", {"name": "generic item", "description": "a generic item with no description" }]
            },
            {
                "component": ["Location"]
            },
            {
                "component": ["View", {"type": "ViewSimple", "name": "Default" }]
            }
        ],
        "behaviours": [
            "Name", "Value", "Pickable", "Droppable"
        ]
    },
    {
        "name": "item_stackable",
        "extends": "item",
        "components": [
            {
                "component": ["Stack", {"size": 100}]
            }
        ]
    }
]