# Energy System Simulation Game

# Change Log

|   Date    |   Author  |   Description    |    Documentation Number   |    Note    |
|-----------|-----------|------------------|---------------------------|------------|
|15/08/2021 |Yifan Song |Project Initiation|            A.01, N03      |    N/A     |
|16/08/2021 |Yifan Song | Game Development |        A.02, N.01         |    N/A     |
|17/08/2021 |Yifan Song | Game Development |        A.03               |    N/A     |
|20/08/2021 |Yifan Song | Game Development |        A.04               |    N/A     |
|22/08/2021 |Yifan Song | Game Development |        A.05, N.02         |    N/A     |





# Documentation

## A.01
- Based on the model I created in the previous stage (Semester 01), I finialize the following attributions:

```
Size of House: 81x56x?
Size of Ground: 150x100
Cell size: 10x10
```

## A.02
The basic logic to place the obejct on the ground is:
1. Convert mouse position into grid position. As the mouse position can be float, this step is to convert float into integer.
2. Convert grid position into grid index. The value of Cell Index and Grid Index are same, but the cell is to store the information. 

Ex.
```
$ Ground Position = (10.9f, 0, 10.9f)
$ | converted into grid position (= float ==> int)|
$ Cell Size = 10
$ Grid Position = (10, 0, 10)
$ | converted into grid index (= cell index) |
$ Grid Index (= Cell Index) = (1, 0 , 1)
```

## A.03
Font: OpenSan-BoldSDF

## A.04
Camera: Perspective
Using keyboard: W A S D or Right Left Up Down to control camera horizontal movement.
Using mouse scroll to control zoom in and zoom out.
Holding mouse right button to rotate camera.

# A.05
CameraMovement Explaination
<strong>Todo</strong>: Vector Projection 


## N.01
To place an object B above the another object A, we need to set the object A to Ground Layer.

## N.02
The playmode test in version 2020.3 is unstable. it might generate some errors while running unit tests. I didn't found any good solution for now. But restarting the game might be working sometimes.

## N.03
As I imported some packages into this proejct, it becomes too big to store in the cloud. This lead to an extra cost for extra storage, which is US$5/month/25GB.