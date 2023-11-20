from pymongo import MongoClient
from bson.objectid import ObjectId

class AnimalShelter(object):
    """ CRUD operations for Animal collection in MongoDB """

    def __init__(self, username, password):
        # Initializing the MongoClient. This helps to 
        # access the MongoDB databases and collections. 
        if username and password:
            print ("Username and Password are:", username, password)
            self.client = MongoClient('mongodb://%s:%s@localhost:29704/AAC' % (username, password))
            # where xxxx is your unique port number
            self.database = self.client['AAC']
            print ("Connection was successful")

# Complete this create method to implement the C in CRUD.
    def create(self, data):
        #return True
        if data is not None:
            if data:
                #replaced '_one' with '_many'
                result = self.database.animals.insert(data)  # data should be dictionary
                
                print(len(data), "inserted record(s)")
                return True if len(data) > 0 else False
        else:
            #data/exception handling
            return False
            raise Exception("Nothing to save, because data parameter is empty")
            
    def read(self, search):
        #used for testing
        #return True
        #data = self.database.animals.find_one()
        #return data
        if search is not None:
            #data = self.database.animals
            #for x in data.find(search):
            #    print(x)
            result = self.database.animals.find(search,{"_id":False}).limit(1000)
            numDocs = self.database.animals.count_documents(search)
            print(numDocs,"documents.")
            return result
        else:
            raise Exception("Could not find data")
        
    ###########################################
    # old method that only returns one record #
    ###########################################
        #if search is not None:
        #    if search:
                #create variable for data to be returned
        #        data = self.database.animals.find_one(search)
                #error handling
        #        if data is None:
        #            print("Could not find record. Try another...")
        #        else:
        #            return data
        #    else:
                #secondary data/exception handling
        #        print("No data entered...")
        #        raise Exception("Exception: Record not found")
        #        return False
            
    def update(self, search, updt):
        #used for testing
        #return True
        #data = self.database.animals.find_one(search)
        #return data
        
        if search is not None: 
            if search:
                #first return the record for a before update
                #data1 = self.database.animals.find_one(search)
                
                #removing '_one' in case we need to find and update many records
                #data1 = self.database.animals.find(search,{"_id":False}).limit(1000)
                #print(data1) # return before value
                
                #replaced '_one' with '_many
                result = self.database.animals.update_many(search, updt)
                #print("Updating Record...")
                
                #find record again and return updated value. removed '_one' here too UPDATE: for testing
                #data2 = self.database.animals.find(search,{"_id":False}).limit(1000)
                print(result.modified_count, "record(s) Updated!")
                return True if result.modified_count > 0 else False
        else:
            #data/exception handling
            print("Check record to update, or record does not exist...")
            raise Exception("Exception: Record not found")
            return False

    def delete(self, data):
        if data is not None:
            if data:
                #create variable to store data and return record for deletions - this is for accidentals so we could re-create record if it was incorrectly entered
                #removed '_one'
                #record = self.database.animals.find(data,{"_id":False}).limit(1000)
                #print(record)
                
                #replaced '_one' with '_many' for multiple records
                record = self.database.animals.delete_many(data)
                #record2 = self.database.animals.find(data,{"_id":False}).limit(100)
                
                #try to return record, successful deletion returns "None" and statement.
                print(record.deleted_count, "record(s) deleted.")                
                return True if record.deleted_count > 0 else False
        else:
            #data/exception handling
            raise Exception("Record not found.")
            return False
            
        
        
        
        
        
        