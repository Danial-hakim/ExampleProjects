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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
/// <summary>
/// Deals with setting the volume for audio mixers
/// </summary>
public class VolumeController : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SoundSlider;

    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("GameMusic", Mathf.Log(1) * 10f);
        mixer.SetFloat("GameSound", Mathf.Log(1) * 10f);
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// When the music switch in the settings menu is changed, the volume field in each audio labelled as game music is also changed
    /// </summary>
    public void UpdateMusicValue(float value)
    {
        mixer.SetFloat("GameMusic", Mathf.Log(value) * 10f);
    }
    /// <summary>
    /// When the audio switch in the settings menu is changed, the volume field in each audio labelled as game sound is also changed
    /// </summary>
    public void UpdateSoundValue(float value)
    {
        mixer.SetFloat("GameSound", Mathf.Log(value) * 10f);
    }
}
