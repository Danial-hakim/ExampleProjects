using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuleDatabases 
{
    private static List<Rule> rules = new List<Rule>();

    public static void AddRule(Rule rule)
    {
        //Debug.Log(rule.obstacleLane);
        if (!HasRuleExisted(rule))
        {
            // Add a new rule to the database
            rules.Add(rule);
        }
        else
        {
            Rule specificRule = rules.Find(r => r.currentLane == rule.currentLane && r.nextLane == rule.nextLane && r.obstacleTag == rule.obstacleTag && r.obstacleLane == rule.obstacleLane);
            List<Rule> similarRule = rules.FindAll(r => r.currentLane == rule.currentLane && r.obstacleTag == rule.obstacleTag && r.obstacleLane == rule.obstacleLane && r.nextLane != rule.nextLane);

            foreach(Rule similarRuleItem in similarRule)
            {
                similarRuleItem.weight--;
                //Debug.Log("Runner lane : " + similarRuleItem.currentLane + " || Obstalce lane : " + similarRuleItem.obstacleLane + " || Next lane : " + similarRuleItem.nextLane + " || Weight : " + similarRuleItem.weight);
            }
            specificRule.weight++;
            
        }
    }

    public static int PredictNextLane(int currentLane, string obstacleTag, int obstacleLane)
    {
        // Find the rule with the highest weight that matches the current state of the game
        int predictedLane = 1;

        List<Rule> similarRule = rules.FindAll(r => r.currentLane == currentLane && r.obstacleTag == obstacleTag && r.obstacleLane == obstacleLane);

        similarRule.Sort((a, b) => b.weight.CompareTo(a.weight));

        if (similarRule.Count > 0)
        {
            foreach(Rule similarRuleItem in similarRule)
            {
                Debug.Log("Runner lane : " + similarRuleItem.currentLane + " || Obstalce lane : " + similarRuleItem.obstacleLane + " || Next lane : " + similarRuleItem.nextLane + " || Weight : " + similarRuleItem.weight);
            }
            if (similarRule[0].weight > 0)
            {
                predictedLane = similarRule[0].nextLane;    
            }
            //Debug.Log("Runner lane : " + similarRule[0].currentLane + " || Obstalce lane : " + similarRule[0].obstacleLane + " || Next lane : " + similarRule[0].nextLane + " || Weight : " + similarRule[0].weight);
        }

        return predictedLane;
    }

    public static bool HasRuleExisted(Rule newRule)
    {
        foreach(Rule rule in rules)
        {
            if(newRule.Matches(rule.currentLane,rule.nextLane,rule.obstacleTag,rule.obstacleLane))
            {
                return true;
            }
        }
        return false;
    }
}
