#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
#define TOUCHEXT_TOUCH_PLATFORMS
#endif

using UnityEngine;
using System.Collections;

/// <summary>
/// Simplifies touch management in basic situations.
/// </summary>
public class TouchExt : MonoBehaviour
{
    static public TouchExt instance;

    void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Is the keyboard active?
    /// </summary>
    static public bool IsKeyboardActive
    {
        get
        {
            return isKeyboardActive;
        }
    }
    static private bool isKeyboardActive = false;


    /// <summary>
    /// Has the touch just began?
    /// </summary>
    /// <Value>
    /// <c>true</c> if touch began; otherwise, <c>false</c>.
    /// </Value>
    static public bool TouchBegan
    {
        get
        {
#if TOUCHEXT_TOUCH_PLATFORMS
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                return true;

            return false;
#else
            if (Input.GetMouseButtonDown(0))
				return true;
			
			return false;
#endif
        }
    }


    /// <summary>
    /// Has the touch just ended?
    /// </summary>
    /// <Value>
    /// <c>true</c> if touch ended; otherwise, <c>false</c>.
    /// </Value>
    static public bool TouchEnded
    {
        get
        {
#if TOUCHEXT_TOUCH_PLATFORMS
            if (Input.touchCount != 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                    return true;

                return false;
            }

            return false;
#else
            if (Input.GetMouseButtonUp(0))
                return true;

            return false;
#endif
        }
    }

    static public Vector2 TouchPosition
    {
        get
        {
#if TOUCHEXT_TOUCH_PLATFORMS
            if (Input.touchCount != 0)
                return Input.GetTouch(0).position;
            
            return Input.mousePosition;
#else
            return Input.mousePosition;
#endif
        }
    }


    /// <summary>
    /// Gets a position on the defined plane when touching.
    /// </summary>
    /// <returns>
    /// The position on the defined plane.
    /// </returns>
    /// <param name='_camera'>
    /// Camera from which the raycast is done.
    /// </param>
    /// <param name='_planeNormal'>
    /// Plane's normal for raycast.
    /// </param>
    /// <param name='_planePosition'>
    /// Plane's position for raycast.
    /// </param>
    /// <param name='_rayPositionShift'>
    /// Additional value to adjust the ray position.
    /// </param>
    static public Vector2 WorldTouchPosition(Camera _camera, Vector3 _planeNormal, Vector3 _planePosition, Vector2 _rayPositionShift)
    {
#if TOUCHEXT_TOUCH_PLATFORMS
        if (Input.touchCount != 0)
        {
            Ray ray = _camera.ScreenPointToRay(TouchExt.TouchPosition + _rayPositionShift);
            Plane p = new Plane(_planeNormal, _planePosition);
            float enter;
            p.Raycast(ray, out enter);
            return ray.origin + ray.direction * enter;
        }
#endif
        {
            // This code executes if the platform is not touch compatible or if touchCount == 0
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition + (Vector3)_rayPositionShift);
            Plane p = new Plane(_planeNormal, _planePosition);
            float enter;
            p.Raycast(ray, out enter);
            return ray.origin + ray.direction * enter;
        }
    }


    /// <summary>
    /// Verifies if a touch began on the collider.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the user has clicked.
    /// </returns>
    /// <param name='_camera'>
    /// Raycast's camera.
    /// </param>
    /// <param name='_collider'>
    /// Collider to test.
    /// </param>
    static public bool TouchBeganOnCollider(Camera _camera, Collider _collider)
    {
        if (TouchExt.TouchBegan)
        {
            Ray ray = _camera.ScreenPointToRay(TouchExt.TouchPosition);
            RaycastHit hit;
            
            if (_collider.Raycast(ray, out hit, Mathf.Infinity))
                return true;
        }

        return false;
    }


    /// <summary>
    /// Verified if a touch is occuring on the collider.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the touch is on the collider.
    /// </returns>
    /// <param name='_camera'>
    /// Raycast's camera.
    /// </param>
    /// <param name='_collider'>
    /// Collider to test.
    /// </param>
    static public bool TouchPositionOnCollider(Camera _camera, Collider _collider)
    {
        Ray ray = _camera.ScreenPointToRay(TouchExt.TouchPosition);
        RaycastHit hit;
        
        if (_collider.Raycast(ray, out hit, Mathf.Infinity))
            return true;

        return false;
    }


    /// <summary>
    /// Is the given rect on screen contains the touch position?
    /// </summary>
    /// <param name='_touchPosition'>
    /// Position du touch (Input.GetTouch().position).
    /// </param>
    static public bool RectContains(Rect _rect, Vector2 _touchPosition)
    {
        // We have to reverse y because touch values are not in the same space coordinates as screen rects.
        if (_rect.Contains(new Vector2(_touchPosition.x, Screen.height - _touchPosition.y)))
            return true;

        return false;
    }

    /*
    /// <summary>
    /// Gère le clavier.
    /// </summary>
    /// <param name='_keyboard'>
    /// Le clavier qui a été initialisé dehors.
    /// </param>
    /// <param name='_maxLength'>
    /// Indique une largeur de texte maximum.
    /// </param>
    public IEnumerator Keyboard(TouchScreenKeyboard _keyboard, int _maxLength = -1)
    {
        // Indiquer que le clavier a été ouvert
        isKeyboardActive = true;
		
		
        // Attendre que le clavier s'ouvre entièrement
        while(!TouchScreenKeyboard.visible)
        {
            yield return null;
        }
		
        while(!_keyboard.done)
        {
            // Limiter la longueur
            if(_maxLength >= 0 && _keyboard.dialogLineId.Length > _maxLength)
                _keyboard.dialogLineId = _keyboard.dialogLineId.Substring(0, 4);
			
            yield return null;
			
			
            // Si le clavier a été ordonné de se ranger, faire comme si le champ a été validé
            if(!TouchScreenKeyboard.visible)
                _keyboard.active = false;
        }
		
		
        // Limiter à nouveau pour plus de sécurité
        if(_maxLength >= 0 && _keyboard.dialogLineId.Length > _maxLength)
            _keyboard.dialogLineId = _keyboard.dialogLineId.Substring(0, 4);
		
		
        // Indiquer que le clavier se ferme
        isKeyboardActive = false;
    }*/
}

