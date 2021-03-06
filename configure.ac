dnl Granularly define the version components
dnl Remember to also update the Display Version
m4_define([tripod_version_major], [0])
m4_define([tripod_version_minor], [0])
m4_define([tripod_version_micro], [1])

dnl set to 0 when doing an official release
m4_define([tripod_version_pre_release], [0])

m4_define([concat], $1$2$3$4)

dnl create base version string
m4_define([tripod_version], concat(
	tripod_version_major.,
	tripod_version_minor.,
	tripod_version_micro))

dnl create the release version
m4_define([tripod_version],
	m4_if(tripod_version_pre_release, [0],
		tripod_version,
		concat(tripod_version, tripod_version_pre_release)))

dnl this can sometimes differ manually
m4_define([tripod_display_version],
	["0.0.1 Alpha"])

m4_define([tripod_api_version],
	[tripod_version_major.tripod_version_minor])

m4_define([tripod_asm_version],
	[tripod_api_version.0.0])

AC_PREREQ(2.52)
AC_INIT([tripod], tripod_version,
	[http://bugzilla.gnome.org/enter_bug.cgi?product=tripod])

AM_INIT_AUTOMAKE([1.9 dist-bzip2 tar-ustar dist-zip foreign])
AM_MAINTAINER_MODE
AM_CONFIG_HEADER([config.h])
AC_SUBST([ACLOCAL_AMFLAGS], ["-I build/m4/shamrock -I build/m4/banshee -I build/m4/tripod -I build/m4/shave \${ACLOCAL_FLAGS}"])

IT_PROG_INTLTOOL([0.35.0])
AC_PROG_LIBTOOL
AC_PROG_INSTALL

dnl How to configure for distcheck
DISTCHECK_CONFIGURE_FLAGS="--disable-docs --enable-release"
AC_SUBST(DISTCHECK_CONFIGURE_FLAGS)

dnl Export Version Info
AC_SUBST([API_VERSION], [tripod_api_version])
AC_SUBST([ASM_VERSION], [tripod_asm_version])
AC_SUBST([DISPLAY_VERSION], [tripod_display_version])

dnl Expansions
SHAMROCK_EXPAND_LIBDIR
SHAMROCK_EXPAND_BINDIR
SHAMROCK_EXPAND_DATADIR

dnl Mono and gmcs
SHAMROCK_CHECK_MONO_MODULE(1.9.1)
SHAMROCK_FIND_MONO_2_0_COMPILER
SHAMROCK_FIND_MONO_RUNTIME
SHAMROCK_CHECK_MONO_2_0_GAC_ASSEMBLIES([	
	System.Data
	Mono.Cairo
	Mono.Posix
	ICSharpCode.SharpZipLib
])

dnl Check for GNOME >= 2.28 to see if we should remove 'Photo Manager' from the app Name, bgo#596242
PKG_CHECK_MODULES(GNOME_LT_2_28, gnome-desktop-2.0 < 2.28, TRIPOD_DESKTOP_NAME="_Name=Tripod Photo Manager", TRIPOD_DESKTOP_NAME="_Name=Tripod")
AC_SUBST(TRIPOD_DESKTOP_NAME)

dnl Monodoc (optional through --disable-docs)
SHAMROCK_CHECK_MONODOC

dnl gio#
TRIPOD_CHECK_GIO_SHARP

dnl NUnit (optional)
SHAMROCK_CHECK_NUNIT

dnl Required dependencies (managed/core)
BANSHEE_CHECK_GTK_SHARP

dnl OS X (optional through --enable-osx)
BANSHEE_CHECK_OSX

PKG_CHECK_MODULES(SQLITE, sqlite3 >= 3.4)

dnl GNOME (optional)
AC_ARG_ENABLE(gnome, 
	AC_HELP_STRING([--disable-gnome], 
		[Disable GNOME support]), 
	, enable_gnome="yes")
if test "x$enable_gnome" = "xyes"; then
	BANSHEE_CHECK_GNOME_SHARP
	AC_PATH_PROG(GCONFTOOL, gconftool-2, no)
	AM_GCONF_SOURCE_2
	AM_CONDITIONAL(ENABLE_GNOME, true)
else
	AM_CONDITIONAL(GCONF_SCHEMAS_INSTALL, false)
	AM_CONDITIONAL(ENABLE_GNOME, false)
fi

dnl i18n
SHAMROCK_CONFIGURE_I18N($PACKAGE)

dnl the bird is the word
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use release configuration]),
	enable_release=yes, enable_release=`(test ! -d .svn && test ! -d .git) && echo "yes" || echo "no"`)
AM_CONDITIONAL(ENABLE_RELEASE, test "x$enable_release" = "xyes")
ENABLE_RELEASE=$enable_release
AC_SUBST(ENABLE_RELEASE)
if test "x$enable_release" = "xyes"; then
	GMCS_FLAGS="-define:RELEASE"
else
	GMCS_FLAGS="-warnaserror"
fi
AC_SUBST(GMCS_FLAGS)

AC_ARG_WITH(vendor-build-id,
	AC_HELP_STRING([--with-vendor-build-id=<vendor-build-id>],
		[Set a vendor build ID, recommended for packagers]),
	[vendor_build_id="$withval"], [
		test -d .git \
			&& vendor_build_id="git-checkout" \
			|| vendor_build_id="source-tarball"
	])
BUILD_VENDOR_ID="$vendor_build_id"
BUILD_HOST_OS="$host_os"
BUILD_HOST_CPU="$host_cpu"
BUILD_TIME=`date +"%F %T %Z"`
AC_SUBST(BUILD_VENDOR_ID)
AC_SUBST(BUILD_HOST_OS)
AC_SUBST(BUILD_HOST_CPU)
AC_SUBST(BUILD_TIME)

SHAVE_INIT([build/m4/shave], [enable])

dnl generated files
AC_OUTPUT([
Makefile

build/Makefile
build/m4/Makefile
build/m4/shave/shave
build/m4/shave/shave-libtool

po/Makefile.in

src/Makefile
src/AssemblyInfo.cs

src/Core/Makefile
src/Core/Tripod.Core/Makefile

src/Clients/Makefile
src/Clients/FlashUnit/Makefile

src/Libraries/Hyena/Makefile
src/Libraries/Makefile
src/Libraries/Mono.Data.Sqlite/Makefile
src/Libraries/SQLite/Makefile
src/Libraries/TagLib/Makefile
src/Libraries/UniqueSharp/generator/Makefile
src/Libraries/UniqueSharp/Makefile
src/Libraries/UniqueSharp/unique/Makefile

extras/Makefile
])

cat <<EOF

${PACKAGE}-${VERSION}

  Build Environment
    Install Prefix:    ${prefix}
    Datadir:           ${expanded_datadir}
    Libdir:            ${expanded_libdir}

    C Compiler:        ${CC}
    Mono C# Compiler:  ${MCS} ${GMCS_FLAGS}
    Mono Runtime:      ${MONO}

  Operating System/Desktop Environment:
    GNOME Support:     ${enable_gnome}
    OSX Support:       ${enable_osx}

  Build/Development:
    Unit Tests:        ${do_tests} (requires nunit >= ${NUNIT_REQUIRED})
    Custom a11y:       ${gtksharp_with_a11y} (requires gtk-sharp >= 2.12.10)
    Release Build:     ${enable_release}
    Vendor Build ID:   ${vendor_build_id}

EOF

if test -d ${expanded_libdir}/${PACKAGE}; then
	if test x$br = xyes; then echo; fi
	echo "  WARNING: An existing tripod install is in ${expanded_libdir}/${PACKAGE}"
	echo "           Remove the existing install before installing this build."
	echo "           Installing over an existing install will cause conflicts!"
	echo 
fi

