@echo off

@SET projectDir=%1%
@SET targetDir=%2%
@SET targetFileName=%3%
@SET configurationName=%4%

@SET configFileName=%projectDir%metacallConnection.config.%configurationName%
@SET targetConfigFileName=%targetDir%metacallConnection.config

@rem echo %configFileName%
@rem echo %targetConfigFileName%

@copy "%configFileName%" "%targetConfigFileName%" 