using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using Moq;
using System;

namespace FluentSerializer.Core.Tests.ObjectMother;

public static class ScanListObjectMother
{
	/// <summary>
	/// Setup the mock of <see cref="IClassMapScanList{TSerializerProfile}"/> to return <paramref name="mapping"/> on scan
	/// </summary>
	public static Mock<IClassMapScanList<TProfile, ISerializerConfiguration>> WithClassMap<TProfile>(this Mock<IClassMapScanList<TProfile, ISerializerConfiguration>> scanListMock,
		Type type, IClassMap mapping)
		where TProfile : ISerializerProfile<ISerializerConfiguration>
	{
		scanListMock
			.Setup(list => list.Scan(It.Is((
					(Type type, SerializerDirection direction) scanFor) => scanFor.type == type))
			)
			.Returns(mapping);

		return scanListMock;
	}

	/// <summary>
	/// Setup the mock of <see cref="IClassMapScanList{TSerializerProfile}"/> to return <paramref name="mappingMock"/>'s object on scan
	/// </summary>
	public static Mock<IClassMapScanList<TProfile, ISerializerConfiguration>> WithClassMap<TProfile>(this Mock<IClassMapScanList<TProfile, ISerializerConfiguration>> scanListMock,
		Type type, IMock<IClassMap> mappingMock)
		where TProfile : ISerializerProfile<ISerializerConfiguration>
	{
		return scanListMock
			.WithClassMap(type, mappingMock.Object);
	}
}