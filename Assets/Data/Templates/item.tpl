[
    {
        "name": "item",
        "components": [
            ["_itemDecl_", {"type": "item", "price": 0, "condition": 100}],
            ["item"],
            ["name", {"name": "generic item", "description": "a generic item with no description" }],
            ["location"],
            ["view", {"type": "ViewSimple", "name": "Default" }]
        ],
        "behaviours": [
			"name", "value", "pickable", "droppable"
		]
    },
    {
        "name": "item_stackable",
        "extends": "item",
        "components": [
            ["Stack", {"size": 100}]
        ]
    }
]