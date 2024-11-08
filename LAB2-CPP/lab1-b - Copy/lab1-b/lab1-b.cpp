
#include <stdio.h>
#include <string>
#include <iostream>
#include <fstream>
#include <vector>
#include <omp.h>
#include <random>
#include <algorithm>sss


using namespace std;
//#pragma omp critical
//		{
//			intSum += threadIntSum;
//			floatSum += threadFloatSum;
//		}

class Employee {
public:
	int ID;
	double Salary;
	std::string Name;
	std::string Hash;

	Employee(int id, double salary, std::string name) {
		ID = id;
		Salary = salary;
		Name = name;
		Hash = RandomString(30);
	}

	static std::string RandomString(int length) {
		const std::string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		//const std::string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		std::string randomString;
		std::default_random_engine generator(std::random_device{}()); // Seed with a unique value
		std::uniform_int_distribution<int> distribution(0, chars.length() - 1);

		for (int i = 0; i < length; i++) {
			randomString += chars[distribution(generator)];
		}

		return randomString;
	}
	/*int CompareTo(const Employee& other) const {
		return Name.compare(other.Name);
	}*/
	int CompareTo(const Employee& other) const {
		if (ID < other.ID) {
			return -1;
		}
		else if (ID > other.ID) {
			return 1;
		}
		else {
			return 0;
		}
	}
};

class ResultMonitor {
private:
	static const int maxSize = 25;
	vector<Employee> result_employees;
	int intSum;
	double floatSum;

public:
	ResultMonitor() {
		result_employees.reserve(maxSize);
	}
	int GetIntSum() const {
		return intSum;
	}

	double GetFloatSum() const {
		return floatSum;
	}


	void AddResultData(const Employee& item, int threadIntSum, double threadFloatSum)
	{
#pragma omp critical
		{
			int insertIndex = -1;
			for (int i = 0; i < result_employees.size(); i++)
			{
				if (item.CompareTo(result_employees[i]) < 0) {
					insertIndex = i;
					break;
				}
			}

			if (insertIndex == -1)
			{

				result_employees.push_back(item);
			}
			else {

				result_employees.insert(result_employees.begin() + insertIndex, item);
				
			}
			intSum += threadIntSum;
			floatSum += threadFloatSum;
		}
	}

	int GetCount() const {
		return result_employees.size();
	}

	const vector<Employee>& GetData() const
	{
		return result_employees;
	}
};

vector<Employee> ReadFromFile(const string& filename) {
	vector<Employee> employees;

	try {
		ifstream reader(filename);
		string line;

		while (getline(reader, line)) {
			size_t delimiterPos = line.find(';');

			if (delimiterPos != string::npos) {
				string name = line.substr(delimiterPos + 2);
				int id = stoi(line.substr(0, delimiterPos));
				double salary = stod(line.substr(delimiterPos + 1));

				Employee emp1(id, salary, name);
				employees.push_back(emp1);
			}
		}
	}
	catch (const exception& e) {
		cerr << "Exception: " << e.what() << endl;
	}

	return employees;
}

void WriteResultsToFile(const string& resultfile, const ResultMonitor& rm, int intSum, double floatSum) {
	ofstream writer(resultfile);
	if (rm.GetCount() != 0)
	{
		writer << "--------------------------------------------------------------------\n";

		for (const auto& b : rm.GetData()) {
			writer << b.ID << "\t" << b.Salary << "\t" << b.Name << "\t" << b.Hash << "\n";


		}

		writer << "---------------------------------------------------------------------\n";
		writer << "Sum of ID: " << intSum << "\n";
		writer << "Sum of Salary: " << floatSum << "\n";
	}
	else
	{
		writer << "No data is filtered!";
	}

}

int main(int argc, char* argv[])
{
	string resultfile = "Results.txt";
	string datafile1 = "Employees.txt";

	vector<Employee> employee_list = ReadFromFile(datafile1);
	int numThreads = 4; // 25/4~ each thread takes 6 objects

	int intSum = 0;
	double floatSum = 0.0;

	ResultMonitor rm;

	omp_set_num_threads(numThreads);

	int dataSize = employee_list.size();
	int chunkSize = dataSize / numThreads;// 6
	int remainder = dataSize % numThreads; 

#pragma omp parallel
	{
		int threadId = omp_get_thread_num();
		int startIdx = threadId * chunkSize + min(threadId, remainder);
		int endIdx = (threadId + 1) * chunkSize + min(threadId + 1, remainder);

		int threadIntSum = 0;
		double threadFloatSum = 0.0;
		int numElements = endIdx - startIdx;
		for (int i = startIdx; i < endIdx; i++)
		{
			if (i < dataSize)
			{
				const Employee& b = employee_list[i];
				if (!isdigit(b.Hash[0]))
				{
					threadIntSum = b.ID;
					threadFloatSum = b.Salary;
					rm.AddResultData(b,threadIntSum,threadFloatSum);
				}
			}
		}
	}
	WriteResultsToFile(resultfile, rm, rm.GetIntSum(), rm.GetFloatSum());


	return 0;
};