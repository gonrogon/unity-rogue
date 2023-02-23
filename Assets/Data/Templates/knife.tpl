{
    "name": "knife",
    "extends": "item",
    "components": [
        {
            "override": "replace",
            "flyweight": true,
            "component": ["ItemDecl", {"category": "weapon/melee", "type": "knife", "price": 100}]
        },
        {
            "override": "replace",
            "component": ["Name", {"name": "knife", "description": "a simple knife"}]
        },
        {
            "override": "replace",
            "component": ["View", {"sprite": "knife"}]
        },
        {
            "component": ["Damage", {"max": 10, "initial": 10, "current": 10}]
        }
    ],
    "behaviours": [
        "Weapon"
    ]
}