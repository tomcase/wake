# Wake

## A command line tool to wake network devices over LAN

If you're anything like me, you have way too many computers in your house. Wake is a tool that allows you to wake any devices on your network without having to get up and power them on manually, provided they have enabled Wake-On-LAN functionality in their BIOS.

### Usage

If the command is run on its own, you'll be prompted to select a device to wake

```
$ wake

Saved NICs:
MyPC: 1A-1A-1A-1A-1A-1A
What's the MAC address of the NIC you want to wake?
```

If the MAC address has not been used before, it'll offer to save it for you for easier re-use. Otherwise, it will attempt to wake it.

Once a MAC address has been saved, it can be accessed quickly as an argument to the `wake` command. You can also pass in the name of the device that you provided when you saved the MAC address.

```bash
# Wake by MAC Address
wake 1A-1A-1A-1A-1A-1A

# Wake by name
wake MyPC
```
