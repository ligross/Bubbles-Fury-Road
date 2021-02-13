using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BonusType {Fast, Slow, Free, Score, Carma, Ghost, Fire, Poison};

public class Bonus
{
	private BonusType _name;
	private int _duration;
	private float _multiplier;
	private float _startTime;

	public Bonus(BonusType type, float value, int duration=0)
	{
		_name = type;
		_multiplier = value;
		_duration = duration;
		if (_duration != 0)
			Activate ();
    }

	public void Activate()
	{
		_startTime = Time.time;
		_duration = 10;
	}

    public bool IsActive()
    {
        return Time.time - (StartTime + Duration) < 0;
    }

	public float TimeTillEnd()
	{
		return StartTime + Duration - Time.time;
	}

	public BonusType Name {
		set { _name = value ;}
		get { return _name; }
	}

	public float Duration {
		get { return _duration; }
	}

	public float Multiplier {
		get { return _multiplier; }
	}

	public float StartTime {
		get { return _startTime; }
	}

	public bool isSpeedBonus()
	{
		return (Name.Equals(BonusType.Fast) || Name.Equals(BonusType.Slow));
	}

	public bool isScoreBonus()
	{
		return (Name.Equals(BonusType.Fast) || Name.Equals(BonusType.Slow) || Name.Equals(BonusType.Score));
	}

}


