/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 16/08/2021
 |  Description: To acquire players input, which is the mouse position on the ground (PC version)
 |  Bugs: N/A
 *=======================================================*/
using System;
using UnityEngine;

public interface IInputController
{
    LayerMask MouseInputMask { get; set; }

    void AddListenerOnPointerDownEvent(Action<Vector3> listener);

    void RemoveListenerOnPointerDownEvent(Action<Vector3> listener);

    void AddListenerOnPointerUpEvent(Action listener);

    void RemoveListenerOnPointerUpEvent(Action listener);

    void AddListenerOnPointerChangeEvent(Action<Vector3> listener);

    void RemoveListenerOnPointerChangeEvent(Action<Vector3> listener);

}