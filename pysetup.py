import sys
import getopt
def main(argv):
	inputfile = ''
	outputfile = ''
	try:
		opts, args = getopt.getopt(argv,"hi:o:",["ifile=","ofile="])
	except getopt.GetoptError:
		print ('decoder.py -i <inputfile> -o <outputfile>')
		sys.exit(2)
	for opt, arg in opts:
		if opt in ("-i", "--ifile"):
			inputfile = arg
		elif opt in ("-o","--ofile"):
			outputfile = arg
		elif opt in ("-h"):
			print("help stuff")
	
if __name__ == "__main__":
	main(sys.argv[1:])
