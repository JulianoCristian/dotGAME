﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Input;
using Duality.Editor;

namespace DotGame
{
	[RequiredComponent(typeof(RigidBody))]
	public class PlayerShip : Component, ICmpUpdatable
	{
		private float turnSpeed = 0.1f;
		private float moveAcceleration = 0.2f;
		private ContentRef<Prefab> laserPrefab = null;

		/// <summary>
		/// This is an awesome XML comment.
		/// </summary>
		[EditorHintRange(0.0f, 0.5f)]
		public float TurnSpeed
		{
			get { return this.turnSpeed; }
			set { this.turnSpeed = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float MoveAcceleration
		{
			get { return this.moveAcceleration; }
			set { this.moveAcceleration = value; }
		}
		public ContentRef<Prefab> LaserPrefab
		{
			get { return this.laserPrefab; }
			set { this.laserPrefab = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			RigidBody body = this.GameObj.GetComponent<RigidBody>();

			float targetRotation = 0.0f;
			if (DualityApp.Keyboard[Key.Left])
				targetRotation = -1.0f;
			else if (DualityApp.Keyboard[Key.Right])
				targetRotation = 1.0f;

			body.AngularVelocity = targetRotation * this.turnSpeed;

			Vector2 targetMovement = Vector2.Zero;
			if (DualityApp.Keyboard[Key.Up])
				targetMovement = -Vector2.UnitY;
			else if (DualityApp.Keyboard[Key.Down])
				targetMovement = Vector2.UnitY;

			body.ApplyLocalForce(targetMovement * this.moveAcceleration * body.Mass);

			if (DualityApp.Keyboard.KeyHit(Key.Space))
			{
				GameObject laserObj = this.laserPrefab.Res.Instantiate();
				laserObj.Transform.Pos = this.GameObj.Transform.Pos;
				laserObj.Transform.Angle = this.GameObj.Transform.Angle;

				RigidBody laserBody = laserObj.GetComponent<RigidBody>();
				laserBody.LinearVelocity = Vector2.FromAngleLength(this.GameObj.Transform.Angle, 10.0f);

				this.GameObj.ParentScene.AddObject(laserObj);
			}
		}
	}
}
