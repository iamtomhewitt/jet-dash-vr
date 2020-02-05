# Code Style 💎

## Braces

Braces should always start on a new line, and be used even if there is one statement:

```c#
if (something)
{
    // Correct
}

if (something) {
    // Incorrect
}
```

## Classes

Written in the ```UpperCamelCase``` form.
See the Camel Case Definition section for classes which have acronyms. 

## Comments

Single line or multiline comment: 

```c# 
// Example
```
```c# 
// Multiline
// example
```

## Conditions

Where applicable, use a Ternary operator for simple statements:

```c#
int number = 5;
bool isLessThanTen = (number < 10) ? true : false; 

// Instead of

int number = 5;
bool isLessThanTen;
if (number < 10)
{
    isLessThanTen = true;
}
else
{
    isLessThanTen = false;
}
```

Switch statements should always have a default statement:

```c#
switch (condition)
{
    case someCase:
        // do something
        break;
    
    case anotherCase:
        // do something else
        break;

    default:
        // do a default, perhaps logging or a default value, or just a comment saying unused
        break
}
```

## Constants

Constants are written in ```CONSTANT_CASE```: 

```c# 
private int SOME_VARIABLE = 5;
```

## Documentation

Documentation should only be used when necessary, for example the method `GetName()` does not need documenting, but a method like `UploadScore()` would perhaps need some documentation on what the method does.
Each class should also have a relevant documentation block before it:

```c#
/// <summary>
/// Uploads a new highscore to Dreamlo using UnityWebRequest
/// </summary>
public IEnumerator UploadScore()
```

## Files & Folders
Organise scripts and folders into the same category. They can either be grouped by a component, or types, such as: 

```c#
Assets
    Models
        Dragon
            Model.fbx
            Map.png
        Wizard
            Model.fbx
            Map.png
    Scripts
        Dragon
            Fly.cs
            Roar.cs
        Wizard
            Movement.cs
            Spells.cs

// Or the following way can be used

Assets
    Dragon
        Model.fbx
        Map.png
        Fly.cs
        Roar.cs
    Wizard
        Model.fbx
        Map.png
        Movement.cs
        Spells.cs
```
The first way is usually preferred, but it depends on the project.

## Methods

Written in the ```UpperCamelCase``` form. Methods should be self describing and only do one thing, for example ```MovePlayer()``` or ```GetHealth()```. If the method does more than one thing, it needs to be refactored.
Other examples:
    
```c#
UploadScoresViaJsonAndHttp();   // NOT: uploadScoresViaJSONAndHTTP();
DecideIfHttpXmlOrHttpJson();    // NOT: decideIfHTTPXMLOrHTTPJSON();
```

Methods should have no more than four parameters. If it requires more, refactor your code (for example, use a Builder or an instance variable).
Parameters should remain on the same line unless its absolutely necessary to put them on a new line (e.g. readability):  

```c#
Debug.Log("Failed to process"
        + " request " + request.GetId()
        + " for user " + user.GetId()
        + " query: '" + query.GetText() + "'");
```

## Namespaces

Don't use ```com.fpsgame.hud.healthbar```, instead use ```FpsGame.Hud.Healthbar```


## Variables

Written in the ```lowerCamelCase``` form. Variables should be self describing. They should be prefixed with ```[SerializeField] private``` (if needed to be exposed in the inspector) or ```private```, and ordered in the following way in a class: `[SerializeField] private <Type>, public <primative>, private <Type>, private <primative>`. For example:

```c#
[SerializeField] private GameObject notificationText;
[SerializeField] private int distance;
[SerializeField] private bool dead;

private Obstacle[] obstacles;
private bool collectedPowerup;
```

Do not use public variables unless absolutely necessary. Variables should be private with a public `Get()` and `Set()` method.

If a GameObject is only present once in a scene, then the following instance pattern should be used

```c#
// GameManager.cs

public static GameManager instance;

private void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    else
    {
        Destroy(this.gameObject);
        return;
    }
}
```
It would be then used in another script:
```c#
GameManager.instance.DoSomething();
```

Variables that need to be private but need to be shown in Inspector should use the `[SerializeField]` before a variable. For the other way around, use `[HideInInspector]`


## Camel Case Defined

Camel casing should follow this rule:

|           Form          |      Correct      |     Incorrect     |
|:-----------------------:|:-----------------:|:-----------------:|
| "XML HTTP request"      | XmlHttpRequest    | XMLHTTPRequest    |
| "new customer ID"       | newCustomerId     | newCustomerID     |
| "supports IPv6 on iOS?" | supportsIpv6OnIos | supportsIPv6OnIOS |
