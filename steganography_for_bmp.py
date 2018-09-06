from __future__ import absolute_import, unicode_literals #[]
import sys
import getopt
from PIL import Image
import numpy as np
import binascii
#this script can both encode text into a bmp file using LSB steganograpy on 1 bit, and decode text encoded in the same manner

#encodes message into selected inputfile, starting from left bottom corner and in reverse 
#RGB order (same order that bmp orders the bytes)
def encode(inputfile,outputfile, message):
	bits = textToBits(message)
	plist = imgToList(inputfile)
	bitCounter=0
	broke = False
	for row in range(len(plist)-1,-1,-1):
		for column in range(len(plist[row])):
			for color in range(2,-1,-1):
				#print(bitCounter)
				if bits[bitCounter] == '1':
					if plist[row][column][color] % 2 == 0:
						plist[row][column][color] = plist[row][column][color] + 1
				else:
					if plist[row][column][color] % 2 == 1:
						plist[row][column][color] = plist[row][column][color] -1
				bitCounter=bitCounter+1;
				if bitCounter == len(bits):
					broke = True
					break
			if broke:
				break
		if broke:
			break
	array = np.asarray(plist)
	im = Image.fromarray(array.astype('uint8'))
	im.save("./"+outputfile,"BMP")
	print("Finished")			
	
#takes all LSB and puts them together and turns into ascii	
def decode(inputfile,outputfile):
	plist = imgToList(inputfile) 
	bits = ''
	for row in range(len(plist)-1,-1,-1): #start from botton left
		for column in range(len(plist[row])):
			for color in range(2,-1,-1): #read in order BGR and not RGB
				if plist[row][column][color] % 2 == 0:
					bits += '0'
				else:
					bits += '1'
	msg=bitsToAscii(bits)
	print("t2b:"+textToBits("a")," opposite:")
	write(outputfile,"w",msg)
	print("Finished")

#ascii characters to a string of bits
def textToBits(text):
	bits = ''
	for char in text:
		ascii = ord(char)
		binary = str(bin(ascii)[2:])
		while True:
			if len(binary) < 8:
				binary="0"+binary
			else:
				break
		bits+= binary
	return bits

#string of bits to ascii
def bitsToAscii(bits):
	byteList=bitsToBytes(bits)
	return bytesToAscii(byteList)

#writes string to a file 
def write(outputfile,type,string):
	try:
		f = open(outputfile,type)
		f.write(string)
		f.close()
	except Exception as inst:
		print(inst)  # the exception instance	

#takes in all the bits and puts them together in pairs of eight from left to right
def bitsToBytes(bits):
	bytelist = ['']
	bitCounter = 0
	byteCounter = 0
	for bit in bits:
		if bitCounter == 8:
			bitCounter = 0
			bytelist.append('')
			byteCounter += 1
		bytelist[byteCounter] += bit
		bitCounter += 1
	return bytelist

#list holding strings of 8 bits turned into string of ascii characters
def bytesToAscii(bytes):
	string = ''	
	for byte in bytes:
		string+=chr(int(byte,2))
	return string

#turn image to array with pixels
def imgToList(img):
	im = Image.open("./"+img)
	return np.array(im).tolist()

#writes pixel array to a file
def arrayToString(plist):
	outputString =''
	for row in range(len(plist)-1,-1,-1):
		outputString+="ROW "+str(row)+":"
		for column in range(len(plist[row])):
			outputString+= str(plist[row][column]) +" , "
		outputString+= "\n"
	return outputString

def main(argv):
	inputfile = ''
	outputfile = 'output.txt'
	message = ''
	mode="decode"
	try:
		opts, args = getopt.getopt(argv,"hi:o:m:",["ifile=","ofile=","message=","mode="])
	except getopt.GetoptError:
		print ('decoder.py -i <inputfile> -o <outputfile>')
		sys.exit(2)
	for opt, arg in opts:
		if opt in ("-i", "--ifile"):
			inputfile = arg
		elif opt in ("-h"):
			print("-h for help\n-i or --ifile= for inputfile\n-o or --ofile= for outputfile\n\thidden message is output for decode\n\tpicture encoded with message is output for encode\n-m or --message for message\n--mode (encode or decode) to choose mode, decode is default")
			return
		elif opt in ("-o", "--ofile"):
			outputfile = arg
		elif opt in ("-m", "--ofile"):
			message = arg
		elif opt in ("--mode"):
			mode = arg
	print ('Input file is ', inputfile)
	print ('Output file is ', outputfile)
	print("Mode is "+mode)
	if inputfile != '' and outputfile != '' and mode =="decode":
		print("Decoding " + inputfile + " ...")
		decode(inputfile,outputfile)
	elif inputfile != '' and outputfile != '' and mode == "encode":
		print("Encoding " + inputfile + " with message " + '"' + message + '"' + " ...")
		encode(inputfile,outputfile,message)

	
if __name__ == "__main__":
	main(sys.argv[1:])

