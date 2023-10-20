import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Read the CSV file
data = pd.read_csv('averageLookup3Tables.csv')

X = data['KeyCount'].tolist()
Y = data['AverageLookup'].tolist()
expected = [2, 2, 2, 2, 2, 2]

plt.scatter(X, Y, label="Average Lookup Attempt")
plt.plot(X, expected, label='Expected Average Lookup Time', color='red')
plt.xlabel('Number of Keys')
plt.ylabel('Average Lookup Time')
plt.grid()
plt.legend()
plt.savefig("Plots/AverageLookup3Tables")
plt.show()

dist = np.linalg.norm(np.array(Y) - np.array(expected))
print(dist)
