# Everything after "#" is a comment.
# A file is a class!

# (optional) icon to show in the editor dialogs:

# (optional) class definition:


# Inheritance:
extends Node


# Member variables.
var _port = 8890
var _host = "localhost"
var err = OK
var _bus : EventBus
var is_host = false



func _ready() -> void:
	call_deferred("set_signals")


	# err = await Noray.connect_to_host(_host, _port)
	# if err != OK:
	#     return err

func set_signals():
	_bus = EventBus.gdscriptBus()
	_bus.onMakeRoom.connect(mkRoom)
	Noray.on_oid.connect(printRm)
	# _bus.onMakeRoom.emit()

func mkRoom(): 
	err = await Noray.connect_to_host(_host, _port)
	if err != OK:
		return err
	Noray.register_host()
	await Noray.on_pid

	err = await Noray.register_remote()
	if err != OK:
		return err

func printRm(roomCode: String):
	print(roomCode)
