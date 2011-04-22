using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<TState, TTrigger> 
		where TState : struct
		where TTrigger : struct
{
	// --------------------------------------------------------
	
	// null effect - does nothing
	public static Action NOP = ( () => {} );
	// null guard - allways true
	public static Func<bool> ALWAYS = ( () => { return true; } );
	
	// --------------------------------------------------------
		
	private class Transition
	{
		public TState fromState { get; private set; }
		public TState toState { get; private set; }
		public TTrigger trigger { get; private set; }
		public Func<bool> guard { get; private set; }
		public Action effect { get; private set; }
			
		// trigger [guard] / effect
		public Transition(TState fromState, TTrigger trigger, Func<bool> guard, Action effect, TState toState)
		{
			this.fromState = fromState;
			this.toState = toState;
			this.trigger = trigger;
				
			this.guard = guard;
			this.effect = effect;
		}
	}
		
	// --------------------------------------------------------

	private class StateAction
	{
		public TState state { get; private set; }
		public Action action { get; private set; }
			
		public StateAction(TState state, Action action)
		{
			this.state = state;
			this.action = action;
		}
	}
		
	// --------------------------------------------------------
	
	public bool initialised { get; private set; }
	public TState state { get; private set; }
		
	private LinkedList<Transition> transitions;
	private LinkedList<StateAction> stateEnterActions;
	private LinkedList<StateAction> stateLeaveActions;
	
	// --------------------------------------------------------

	public FiniteStateMachine ()
	{
		initialised = false;
		transitions = new LinkedList<Transition>();
		stateEnterActions = new LinkedList<StateAction>();
		stateLeaveActions = new LinkedList<StateAction>();
	}
		
	public void addTransitions(TState[] fromStates, TTrigger trigger, Func<bool> guard, Action effect, TState toState)
	{
		foreach (TState fromState in fromStates)
		{
			addTransition(fromState, trigger, guard, effect, toState);	
		}
	}

	public void addTransition(TState fromState, TTrigger trigger, Func<bool> guard, Action effect, TState toState)
	{
		DebugHelper.assert(!initialised, "already started");
			
		Transition t = new Transition(fromState, trigger, guard, effect, toState);
		transitions.AddLast(t);
	}
		
	public void addStateEnterAction(TState state, Action enterAction)
	{
		DebugHelper.assert(!initialised, "already started");
			
		StateAction sa = new StateAction(state, enterAction);
		stateEnterActions.AddLast(sa);
	}
		
	public void addStateLeaveAction(TState state, Action leaveAction)
	{
		DebugHelper.assert(!initialised, "already started");
			
		StateAction sa = new StateAction(state, leaveAction);
		stateLeaveActions.AddLast(sa);
	}
	
	// calls enter functions of start state
	public void start(TState startState)
	{
		DebugHelper.assert(!initialised, "already started");
			
		initialised = true;
		state = startState;
		callStateActionsFromList(startState, stateEnterActions);
	}
		
	// NOTE state changes to toState after effect gets executed
	public void notifyTrigger(TTrigger trigger)
	{
		DebugHelper.assert(initialised, "you need to start the statemachine first");
			
		foreach (Transition t in transitions)
		{
			if (t.fromState.Equals(state) && t.trigger.Equals(trigger))
			{
				if (t.guard())
				{
					t.effect();
					
					// Debug.Log(this + ": " + t.fromState + " -> " + t.toState);
					
					switchState(t.toState);
						
					return;
				}
			}
		}
	}
		
	// --------------------------------------------------------
		
	private void switchState(TState nextState)
	{
		if (!state.Equals(nextState))
		{
			// switch needed
			callStateActionsFromList(state, stateLeaveActions);
			state = nextState;
			callStateActionsFromList(nextState, stateEnterActions);
		}
	}
		
	private void callStateActionsFromList(TState state, LinkedList<StateAction> stateActions)
	{
		foreach (StateAction sa in stateActions)
		{
			if (sa.state.Equals(state))
			{
				sa.action();	
			}
		}
	}
}
