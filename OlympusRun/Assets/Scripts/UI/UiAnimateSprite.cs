/*
Olympus Run - A game made as part of the ARGO Project at SETU Carlow
Copyright (C) 2023 Caroline Percy <lineypercy@me.com>, Patrick Donnelly <patrickdonnelly3759@gmail.com>, Izabela Zelek <izabelawzelek@gmail.com>, Danial-hakim <danialhakim01@gmail.com>, Adrien Dudon <dudonadrien@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimateSprite : MonoBehaviour
{
    private GameObject _uiRunner;
    private Image _image;

    public Sprite[] SpriteArray;
    public float Speed = .75f;

    private int IndexSprite;


    private void Start()
    {
        _uiRunner = GameObject.Find("UIRunner");
        _image = _uiRunner.GetComponent<Image>();
        StartCoroutine(PlayAnimation());
    }

    private void Update()
    {
        if (_uiRunner == null || _image == null)
        {
            Debug.Log("Null reference - cannot find UIRunner");
            _uiRunner = GameObject.Find("UIRunner");
            _image = _uiRunner.GetComponent<Image>();
        }
    }

    /// <summary>
    /// Plays the menu animation
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(Speed);
        if (IndexSprite >= SpriteArray.Length)
        {
            IndexSprite = 0;
        }
        _image.sprite = SpriteArray[IndexSprite];
        IndexSprite += 1;
        StartCoroutine(PlayAnimation());
    }
}
