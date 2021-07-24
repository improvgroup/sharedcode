// <copyright file="DependencyRegister.cs" company="improvGroup, LLC">
//     Copyright � 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.DependencyInjection
{
	using Microsoft.Extensions.DependencyInjection;

	using System;

	/// <summary>
	/// The dependency register class. Implements the <see cref="IDependencyRegister" />.
	/// </summary>
	/// <seealso cref="IDependencyRegister" />
	public class DependencyRegister : IDependencyRegister
	{
		/// <summary>
		/// The service collection
		/// </summary>
		private readonly IServiceCollection serviceCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyRegister" /> class.
		/// </summary>
		/// <param name="serviceCollection">The service collection.</param>
		public DependencyRegister(IServiceCollection serviceCollection) => this.serviceCollection = serviceCollection;

		/// <inheritdoc />
		void IDependencyRegister.AddScoped<TService>() => this.serviceCollection.AddScoped<TService>();

		/// <inheritdoc />
		void IDependencyRegister.AddScoped<TService, TImplementation>() => this.serviceCollection.AddScoped<TService, TImplementation>();

		/// <inheritdoc />
		void IDependencyRegister.AddScopedForMultiImplementation<TService, TImplementation>() =>
			this.serviceCollection
				.AddScoped<TImplementation>()
				.AddScoped<TService, TImplementation>(
					s =>
						s.GetService<TImplementation>()
							?? throw new ArgumentException("Resolved TImplementation service instance cannot be null.", nameof(TImplementation)));

		/// <inheritdoc />
		void IDependencyRegister.AddSingleton<TService>() => this.serviceCollection.AddSingleton<TService>();

		/// <inheritdoc />
		void IDependencyRegister.AddSingleton<TService, TImplementation>() => this.serviceCollection.AddSingleton<TService, TImplementation>();

		/// <inheritdoc />
		void IDependencyRegister.AddTransient<TService>() => this.serviceCollection.AddTransient<TService>();

		/// <inheritdoc />
		void IDependencyRegister.AddTransient<TService, TImplementation>() => this.serviceCollection.AddTransient<TService, TImplementation>();

		/// <inheritdoc />
		void IDependencyRegister.AddTransientForMultiImplementation<TService, TImplementation>() =>
			this.serviceCollection
				.AddTransient<TImplementation>()
				.AddTransient<TService, TImplementation>(
					s =>
						s.GetService<TImplementation>()
							?? throw new ArgumentException("Resolved TImplementation service instance cannot be null.", nameof(TImplementation)));
	}
}
