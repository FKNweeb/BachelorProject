import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import mplcursors
import os

def generate_pareto_curve(csv_file_path, output_csv_file_path, tableCount, keyCount):
    if not os.path.exists("Plots"):
        os.makedirs("Plots")
    if not os.path.exists("CSV_Files"):
        os.makedirs("CSV_Files")
    if not os.path.exists("CSV_Files/HowFilled"):
        os.makedirs("CSV_Files/HowFilled")
    if not os.path.exists("CSV_Files/HowFilled/Pareto"):
        os.makedirs("CSV_Files/HowFilled/Pareto")
    
    data = pd.read_csv(csv_file_path)

    sorted_data = data.sort_values(by=['HowFilled', 'AverageLookUp'], ascending=[False, True])

    best_lookup = float('inf')
    pareto_points = []
    for point in sorted_data.itertuples(index=False):
        if point.AverageLookUp <= best_lookup:
            pareto_points.append(point)
            best_lookup = point.AverageLookUp

    pareto_df = pd.DataFrame(pareto_points)

    plt.figure(figsize=(10, 6))
    plt.scatter(data['HowFilled'], data['AverageLookUp'], color='blue', label='Data Points')
    plt.scatter(pareto_df['HowFilled'], pareto_df['AverageLookUp'], color='red', marker='o', label=f'Pareto Front for {tableCount} tables, {keyCount} KeyCount')
    plt.xlabel('HowFilled')
    plt.ylabel('AverageLookUp')
    plt.title(f'Pareto Curve for {tableCount} tables, {keyCount} KeyCount')
    plt.legend()

    pareto_df.to_csv(output_csv_file_path, index=False)

def generate_plot(csv_filename, table_count, keyCount):
    plot_dir = 'Plots'
    pareto_dir = os.path.join(plot_dir, 'Pareto')

    if not os.path.exists(pareto_dir):
        os.makedirs(pareto_dir)
    data = pd.read_csv(csv_filename)

    X = data['HowFilled'].tolist()
    Y = data['AverageLookUp'].tolist()
    table_sizes = [data[f'TableSize{i}'].tolist() for i in range(1, table_count + 1)]

    fig, ax = plt.subplots()
    ax.scatter(X, Y, color='red', marker='o', label=f'Pareto Curve for {table_count} tables')


    labels = [''.join(f"table size {i+1}: {size},\n" for i, size in enumerate(sizes)) for sizes in zip(*table_sizes)]
    mplcursors.cursor(hover=True).connect("add", lambda sel: sel.annotation.set_text(labels[sel.index]))

    ax.set_xlabel('How Filled')
    ax.set_ylabel('Average Lookup Time')
    ax.set_title(f"Average Lookup Time vs How Filled for {table_count} Tables, {keyCount} KeyCount")
    ax.grid(True)
    ax.legend()

    plot_filename = os.path.join('Plots/Pareto', f'GenericHowFilled{keyCount}_{table_count}_Tables.png')
    plt.savefig(plot_filename)
    plt.close(fig)

# Define table range 
start_table_count = 2
end_table_count = 10

# Loop through all our tables, and generate new plots
# for i in range(start_table_count, end_table_count + 1):
#    csv_filename = f'CSV_Files/HowFilled/GenericHowFilled_{i}tables.csv'
#    csv_File_Out = f'CSV_Files/HowFilled/Pareto/GenericParetoData_{i}tables.csv'
#    generate_pareto_curve(csv_filename, csv_File_Out, i)

# Another loop to show plots without all the other data points
#for i in range(start_table_count, end_table_count + 1):
#    csv_file = f'CSV_Files/HowFilled/Pareto/GenericParetoData_{i}tables.csv'
#    generate_plot(csv_file, i)

keyCount = [1000, 5000, 10000, 20000, 50000, 100000]

for i in range(start_table_count, end_table_count + 1):
    for j in keyCount:
        csv_filename = f'CSV_Files/HowFilled/GenericHowFilled{j}_{i}tables.csv'
        csv_File_Out = f'CSV_Files/HowFilled/Pareto/GenericParetoData{j}_{i}tables.csv'
        generate_pareto_curve(csv_filename, csv_File_Out, i, j)

for i in range(start_table_count, end_table_count + 1):
    for j in keyCount:
        csv_file = f'CSV_Files/HowFilled/Pareto/GenericParetoData{j}_{i}tables.csv'
        generate_plot(csv_file, i, j)