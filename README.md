# Arcade Jet Flight Example

This project is an example of a simple arcade style flight action game airplane. It's controlled entirely through Rigidbody physics, so you get all perks (and cons) of physics in addition to a very straightforward and easy to handle plane. The code is heavily commented, and you are encouraged to play with values or re-organize as needed.

Built in **Unity 2017.3**.

![gif](./Screenshots/flying.gif)

## Download

You can either clone the repository or [download the asset package](./ArcadeJetFlightExample.unitypackage) located in the root.

## Controls

The default controls in the example project are built around an Xbox One controller and a typical "Ace Combat" control scheme. If you have another gamepad, you may have to adjust the inputs.

Note that if you import the Asset Package, you will have to set up some additional controls. The first is a **Yaw** axis. It is as single axis that controls the rudder of the plane. The second and third are **ThrottleUp** and **ThrottleDown** respectively. These are simple buttons, and are used to control the speed. There is no axis to directly control the throttle.

## The Jet
![screenshot](./Screenshots/jet.png)

The jet consists of two components: **StickInput** and **JetMovement**. Both are very simple and scripts, but they are written in a specific way to facilitate the target control scheme.

### Jet Movement

The first thing you must do is assign an input. Jet movement requires a **StickInput** component in order to figure out how to apply the forces it has. Also worth noting is that the default values assume a Rigidbody with a mass of 100 and colliders in a rough airplane shape.

#### Flying smoothly
![screenshot](./Screenshots/banked.png)

If you look at the prefab, you'll notice that it has high drag values. Without such a high drag, the plane would slip all over the place. This can be desirable for certain situations, but in general for a fast action oriented flight game you want tight and predictable controls. However, with such a high drag, the forces applied to it must be smoothly ramped up and down. If not, the same forces that allow the plane to change directions so quickly, will also make the plane accelerate and decelerate almost instantly (i.e., 0-1000 in a second) in response to throttle changes.

To get around this, the jet has a target throttle and a true throttle. The target throttle is simply the user's input and what they're expecting to happen. The true throttle is a "fake" throttle that moves toward the target throttle at a rate determined by the "acceleration" value of the jet. The higher the acceleration, the faster the plane will respond to changes in speed.

#### Banking

A common property of jets is that when they are banked (nose pointed at the horizon, but wings not level), they will turn slightly in the direction they are banked. This is because of aerodynamic force, but because those are not simulated here, we need to fake this bank force.

To get the plane to do something *like* it, a yaw force is applied in the direction of the bank. For example, if a plane is banked to the right, it will yaw to the right. This will also pull the nose downwards, requiring the pilot to give a slight pitch up in order to maintain level flight in a turn, which is something happens to be fairly close to reality. So that's cool.

![screenshot](./Screenshots/vectors.png)

The way this gets done though is through some not so obvious Transform vector usage. Getting the forward, right, or up of a transform is a very common operation. Using the Y value of the right, we can tell how "high" the right wing is. A value of -1/1 implies the plane is flying sideways. It also automatically takes care of cases where the plane is flying straight up or down because in those situations your right would have negligible Y value.

That number is turned into the "bank factor" so that at 90 degrees bank to the right, it will apply the bank torque at full power to the plane.
