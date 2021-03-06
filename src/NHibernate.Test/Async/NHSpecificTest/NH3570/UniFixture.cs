﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH3570
{
	using System.Threading.Tasks;
	[TestFixture]
	public class UniFixtureAsync : BugTestCase
	{
		private Guid id;

		[Test]
		[KnownBug("NH-3570")]
		public async Task ShouldNotSaveRemoveChildAsync()
		{
			var parent = new UniParent();
			parent.Children.Add(new UniChild());
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					id = (Guid) await (s.SaveAsync(parent));
					parent.Children.Clear();
					parent.Children.Add(new UniChild());
					await (tx.CommitAsync());
				}
			}
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.That((await (s.GetAsync<UniParent>(id))).Children.Count, Is.EqualTo(1));
					Assert.That((await (s.CreateCriteria<UniChild>().ListAsync())).Count, Is.EqualTo(1));
				}
			}
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from UniChild").ExecuteUpdate();
					s.CreateQuery("delete from UniParent").ExecuteUpdate();
					tx.Commit();
				}
			}
		}
	}
}