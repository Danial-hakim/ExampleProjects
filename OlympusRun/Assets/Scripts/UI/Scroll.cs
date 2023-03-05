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

using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    public float speed =0f;

    [SerializeField]
    float width = 3410;
    public bool scrollLeft = false;
    public bool scrollRight = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scrollLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= -width)
            {
                SetPosition(width);
            }
        }
        else if(scrollRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= width)
            {
                SetPosition(-width);
            }
        }
    }

    /// <summary>
    /// Sets the scroll speed
    /// </summary>
    /// <param name="t_speed"></param>
    public void SetSpeed(float t_speed)
    {
        speed = t_speed;
    }

    /// <summary>
    /// Sets the position
    /// </summary>
    /// <param name="t_position"></param>
    public void SetPosition(float t_position)
    {
        transform.position = new Vector3(t_position, transform.position.y, transform.position.z); 
    }
}
