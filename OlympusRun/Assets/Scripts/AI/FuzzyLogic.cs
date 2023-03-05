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

/// <summary>
/// The Implementation of Fuzzy Logic in the Runner AI
/// </summary>
public class FuzzyLogic : MonoBehaviour
{
    private float _lane1, _lane2, _lane3;
    private float _none, _avoidObs, _jumpObs, _slideObs;

    private float _run, _right, _left, _jump, _slide;

    private float _nextAction;

    /// <summary>
    /// Figures out the next action the runner using Fuzzy Logic.
    /// </summary>
    /// <param name="currentLane">The runner's current lane.</param>
    /// <param name="currentObstacle">The obstacle the runner is seeing.</param>
    /// <returns>The next action needed to be taken.</returns>
    public float SetMemberShipFunctions(float currentLane, float currentObstacle)
    {
        _lane1 = FuzzyTriangle(currentLane, -0.33f,0.0f, 0.33f);
        _lane2 = FuzzyTrapezoid(currentLane, 0.33f, 0.33f, 0.66f, 0.66f);
        _lane3 = FuzzyGrade(currentLane, 0.66f, 1.0f);

        _none = FuzzyTriangle(currentObstacle, -0.25f, 0, 0.25f);
        _avoidObs = FuzzyTrapezoid(currentObstacle, 0.25f, 0.25f, 0.5f, 0.5f);
        _jumpObs = FuzzyTrapezoid(currentObstacle, 0.5f, 0.5f, 0.75f, 0.75f);
        _slideObs = FuzzyGrade(currentObstacle, 0.75f, 1.0f);

        _run = FuzzyOR(FuzzyOR(FuzzyAND(_none, _lane1), FuzzyAND(_none, _lane2)), FuzzyAND(_none, _lane3));
        _right = FuzzyOR(FuzzyAND(_avoidObs, _lane1), FuzzyAND(_avoidObs, _lane2));
        _left = FuzzyAND(_avoidObs, _lane3);
        _jump = FuzzyOR(FuzzyOR(FuzzyAND(_jumpObs, _lane1), FuzzyAND(_jumpObs, _lane2)), FuzzyAND(_jumpObs, _lane3));
        _slide = FuzzyOR(FuzzyOR(FuzzyAND(_slideObs, _lane1), FuzzyAND(_slideObs, _lane2)), FuzzyAND(_slideObs, _lane3));

        _nextAction = (_run * 1 + _right * 2 + _left * 3 + _jump * 4 + _slide * 5) / (_run + _right + _left + _jump + _slide);
        _nextAction = Mathf.Round(_nextAction);

        return _nextAction;
    }
    
    /// <summary>
    /// Creates a grade on a Graph.
    /// </summary>
    /// <param name="value">The Value that needs to be checked.</param>
    /// <param name="x0">The start of the grade.</param>
    /// <param name="x1">The end of the grade.</param>
    /// <returns>The weight of the value through the grade</returns>
    private float FuzzyGrade(float value,float x0,float x1)
    {
        float result = 0f;
        float x = value;

        if(x <= x0)
        {
            result = 0;
        }

        else if (x > x1)
        {
            result = 1;
        }
        else
        {
            result = ((x - x0) / (x1 - x0));
        }
        return result; 
    }

    /// <summary>
    /// Creates a triangle on a Graph.
    /// </summary>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="x0">Bottom left of the triangle</param>
    /// <param name="x1">Top of the triangle</param>
    /// <param name="x2">Bottom right of the triangle</param>
    /// <returns>The weight of the value through the triangle.</returns>
    private float FuzzyTriangle(float value,float x0,float x1,float x2)
    {
        float result = 0;
        float x = value;

        if ((x <= x0) || (x >= x2))
        {
            result = 0;
        }
        else if (x == x1)
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = ((x - x0) / (x1 - x0));
        }
        else
        {
            result = ((x2 - x) / (x2 - x1));
        }
        return result;
    }

    /// <summary>
    /// Creates a trapezoid on a Graph.
    /// </summary>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="x0">Bottom left of trapezoid</param>
    /// <param name="x1">Top left of trapezoid</param>
    /// <param name="x2">Top right of trapezoid</param>
    /// <param name="x3">Bottom right of trapezoid</param>
    /// <returns></returns>
    private float FuzzyTrapezoid(float value, float x0, float x1, float x2, float x3)
    {
        float result = 0;
        float x = value;

        if ((x <= x0) || (x >= x3))
        {
            result = 0;
        }
        else if ((x >= x1) && (x <= x2))
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = ((x - x0) / (x1 - x0));
        }
        else
        {
            result = ((x3 - x) / (x3 - x2));
        }
        return result;
    }

    /// <summary>
    /// Returns the smallest number of the two.
    /// </summary>
    /// <param name="A">First value to check</param>
    /// <param name="B">Second value to check</param>
    /// <returns>The lower value of A and B</returns>
    private float FuzzyAND(float A, float B)
    {
        return Mathf.Min(A, B);
    }

    /// <summary>
    /// Returns the best number of the two.
    /// </summary>
    /// <param name="A">First number to check</param>
    /// <param name="B">Second number to check</param>
    /// <returns>The higher value of A and B</returns>
    private float FuzzyOR(float A, float B)
    {
        return Mathf.Max(A, B);
    }
}
