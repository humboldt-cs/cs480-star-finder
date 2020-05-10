# cs480-star-finder

## Table of Contents
1. [Overview](#Overview)
1. [Product Spec](#Product-Spec)
1. [Wireframes](#Wireframes)
2. [Schema](#Schema)

## Overview
### Description
Star-Finder is a Unity application that will allow users to explore an accurate simulation of the night sky on their mobile devices. Users can view the stars, planets, and other deep sky objects that are above their location. Star-Finder will also incorporate AR features, allowing the user to aim their phone in any direction (whether the sky or the ground) and see what stars and constellations are located in that direction.

### App Evaluation
- **Category:** Astronomy/recreational.
- **Mobile:** Android.
- **Story:** Allows the user to point their phone in any direction, and use AR inputs to show them what celestial objects are located in
that direction.
- **Market:** Astronomers, both professional and amateur.
- **Habit:** Could be used as often or as little as the user cares to. 
- **Scope:** We would first want to get a few test objects up and running to represent 20 or so objects, while configuring the camera to
move in response to AR inputs. Later, we could add more objects, or perhaps touch up the visual representation of the objects.

## Product Spec

### 1. User Stories (Required and Optional)


**Required Must-have Stories**

* Display an accurate simulation of bright objects in the celestial sphere (stars, planets, nebulas, galaxies)
* Change the orientation of the horizon based on user geolocation
* Integrate AR features to allow the user to explore the night sky in real time

**Optional Nice-to-have Stories**

* Add a search option to allow the user to find particular objects of interest
* Allow the user to view constellation outlines and adjust other visual settings
* Give scientific information back to the user when selecting a night sky object (star class, size, magnitude etc.)

### 2. Screen Archetypes

* Main Screen (Night Sky Viewer)
   * Display an accurate simulation of bright objects in the celestial sphere (stars, planets, nebulas, galaxies)
   * Change the orientation of the horizon based on user geolocation
   * Integrate AR features to allow the user to explore the night sky in real time
* Search Screen
   * Add a search option to allow the user to find particular objects of interest
* Settings Screen
   * Allow the user to view constellation outlines and adjust other visual settings
* Details Screen
   * Give scientific information back to the user when selecting a night sky object (star class, size, magnitude etc.)

### 3. Navigation

**Tab Navigation** (Tab to Screen)

* Star-Finder will not use tab navigation

**Flow Navigation** (Screen to Screen)

* Night Sky View Screen
   * Magnifying Glass Icon -> Search Screen
   * Hamburger Menu Icon -> Settings Screen
   * Selecting Night Sky Object -> Details Screen
* Search Screen
   * Back Arrow Button -> Night Sky View Screen (No Search)
   * Search Button -> Night Sky View Screen (Search for Object)
* Settings Screen
   * Save Settings Button -> Night Sky View Screen
* Details Screen
   * Back Arrow Button -> Night Sky View Screen
   
## Progress .gifs

<img src="https://i.imgur.com/nB30wEZ.gif">

<img src ="https://i.imgur.com/9DbdLoJ.gif">

<img src ="https://i.imgur.com/U4KE7lu.gif">

<img src ="https://i.imgur.com/blB4h3X.gif">

## Wireframes
<img src="https://github.com/humboldt-cs/cs480-star-finder/blob/master/StarFinderWireframe.jpg" width=1000>

### [BONUS] Digital Wireframes & Mockups

<img src="https://i.imgur.com/qPFh5Uk.png" width=1000>

### [BONUS] Interactive Prototype

<img src="https://i.imgur.com/LF05z61.gif">

## Schema 

### Models

MainScene

| Property      | Type          | Description   |
| ------------- | ------------- | ------------- |
| Main Camera  | GameObject  | Serves as user's POV  | 
| Directional Light  | GameObject  | Serves to determine the lighting of the MainScene  | 
| Horizon  | GameObject | Serves as the "Earth" and "Horizon", to let the user know where the constellations are relative to their POV |
| UI | GameObject | Serves as user interface in main star view, allows user access to the menu systems |
| Menu | Prefab | 2D canvas with child elements (buttons, toggles etc.) to allow the user to change app settings
| Star | Prefab | Represents a star in the night sky |
| EventSystem | GameObject | Responsible for processing and handling events in a Unity scene |

### Networking

* Main Scene
  * (Read/GET) Query local SQL database populated with Yale Bright Star Catalog data
  * (Update/PUT) Populate local SQL database with Yale Bright Star Catalog data
  * (Read/GET) Query NASA JPL's HORIZON telnet database to grab ephemerides data on solar system objects

### Credit

Toggle On/Off Icons by Icons8.com
