ASSEMBLY = Tripod.Core
TARGET = library
LINK = $(REF_TRIPOD_CORE)
SOURCES =  \
	Tripod.Base/Core.cs \
	Tripod.Base/SafeUri.cs \
	Tripod.Collection.Database/DatabaseListModel.cs \
	Tripod.Collection.Database/DatabasePhoto.cs \
	Tripod.Collection/CacheableItem.cs \
	Tripod.Collection/Photo.cs \
	Tripod.Gui.Collection/PhotoListView.cs \
	Tripod.Gui.Collection/PhotoViewChild.cs \
	Tripod.Jobs/ThreadPoolJob.cs

RESOURCES =  \
	Resources/contributors.xml \
	Resources/COPYING

include $(top_srcdir)/build/build.mk

EXTRA_DIST += Tripod.Core.dll.config
module_SCRIPTS += Tripod.Core.dll.config
